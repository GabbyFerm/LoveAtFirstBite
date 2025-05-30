using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Votes.Commands.CreeateVote;
using Application.Votes.Handlers;
using Application.Votes.Dtos;
using Application.Votes.Queries;
using Domain.Common;
using Domain.Models;
using AutoMapper;
using MediatR;
using Moq;
using NUnit.Framework;

namespace LoveAtFirstBite.Tests.Handlers
{
    [TestFixture]
    public class CastVoteCommandHandlerTests
    {
        private Mock<IGenericRepository<Vote>> _voteRepoMock;
        private Mock<IGenericRepository<User>> _userRepoMock;
        private Mock<IGenericRepository<Restaurant>> _restaurantRepoMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IMediator> _mediatorMock;
        private CastVoteCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _voteRepoMock = new Mock<IGenericRepository<Vote>>();
            _userRepoMock = new Mock<IGenericRepository<User>>();
            _restaurantRepoMock = new Mock<IGenericRepository<Restaurant>>();
            _mapperMock = new Mock<IMapper>();
            _mediatorMock = new Mock<IMediator>();

            _handler = new CastVoteCommandHandler(
                _voteRepoMock.Object,
                _userRepoMock.Object,
                _restaurantRepoMock.Object,
                _mapperMock.Object,
                _mediatorMock.Object
            );
        }

        private static DateTime TodayDateOnly() => DateTime.UtcNow.Date;

        [Test]
        public async Task ExistingVote_SameRestaurantAndRound_ReturnsFailure()
        {
            // Arrange
            var cmd = new CastVoteCommand(1, 42, round: 1);

            _userRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(OperationResult<User>.Success(new User
                {
                    UserId = 42,
                    UserName = "u",
                    UserEmail = "u@example.com",
                    PasswordHash = "hash"
                }));

            _restaurantRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(OperationResult<Restaurant>.Success(new Restaurant
                {
                    RestaurantId = 1,
                    RestaurantName = "R1",
                    Address = "Addr1",
                    CreatedByUserId = 42,
                    CreatedByUser = new User
                    {
                        UserId = 42,
                        UserName = "u",
                        UserEmail = "u@example.com",
                        PasswordHash = "hash"
                    }
                }));

            var existing = new Vote
            {
                VoteId = 10,
                UserId = 42,
                RestaurantId = 1,
                VoteDate = TodayDateOnly(),
                Round = 1
            };
            _voteRepoMock
                .Setup(r => r.AsQueryable())
                .Returns(new List<Vote> { existing }.AsQueryable());

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(
                "already voted for this restaurant",
                result.ErrorMessage ?? result.Errors?.FirstOrDefault() ?? string.Empty
            );
        }

        [Test]
        public async Task ExistingVote_DifferentRestaurant_UpdatesAndReturnsSuccess()
        {
            // Arrange
            var cmd = new CastVoteCommand(2, 42, round: 1);

            _userRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(OperationResult<User>.Success(new User
                {
                    UserId = 42,
                    UserName = "u",
                    UserEmail = "u@example.com",
                    PasswordHash = "hash"
                }));

            _restaurantRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(OperationResult<Restaurant>.Success(new Restaurant
                {
                    RestaurantId = 2,
                    RestaurantName = "R2",
                    Address = "Addr2",
                    CreatedByUserId = 42,
                    CreatedByUser = new User
                    {
                        UserId = 42,
                        UserName = "u",
                        UserEmail = "u@example.com",
                        PasswordHash = "hash"
                    }
                }));

            var original = new Vote
            {
                VoteId = 11,
                UserId = 42,
                RestaurantId = 1,
                VoteDate = TodayDateOnly(),
                Round = 1
            };
            _voteRepoMock
                .Setup(r => r.AsQueryable())
                .Returns(new List<Vote> { original }.AsQueryable());

            _voteRepoMock
                .Setup(r => r.UpdateAsync(
                    It.Is<Vote>(v => v.RestaurantId == 2),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(OperationResult<Vote>.Success(original));

            var mappedDto = new VoteDto
            {
                RestaurantId = 2,
                VoteDate = original.VoteDate,
                Round = 1
            };
            _mapperMock
                .Setup(m => m.Map<VoteDto>(It.IsAny<Vote>()))
                .Returns(mappedDto);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(2, result.Data!.RestaurantId);
            _voteRepoMock.Verify(r => r.UpdateAsync(
                It.IsAny<Vote>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task RoundGreaterThanOne_NonTiedRestaurant_ReturnsFailure()
        {
            // Arrange
            var cmd = new CastVoteCommand(5, 42, round: 2);

            var prev = new List<TodayVoteTallyDto> {
                new TodayVoteTallyDto { RestaurantId = 3, RestaurantName = "R3", VoteCount = 1, IsLeader = true },
                new TodayVoteTallyDto { RestaurantId = 4, RestaurantName = "R4", VoteCount = 1, IsLeader = true }
            };
            _mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<GetTodayVoteTallyQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(OperationResult<List<TodayVoteTallyDto>>.Success(prev));

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            StringAssert.Contains(
                "only vote among the tied restaurants",
                result.ErrorMessage ?? result.Errors?.FirstOrDefault() ?? string.Empty
            );
        }
    }
}
