using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Votes.Commands.CreeateVote;
using Application.Votes.Handlers;
using Application.Votes.Dtos;
using AutoMapper;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Application.Interfaces;
using Application.Common.Mappings;
using MediatR;
using Application.Commen.Mappings;

namespace LoveAtFirstBite.Tests.Handlers
{
    [TestFixture]
    public class VoteTest
    {
        private LoveAtFirstBiteDbContext _context;
        private IGenericRepository<Vote> _voteRepo;
        private IGenericRepository<User> _userRepo;
        private IGenericRepository<Restaurant> _restaurantRepo;
        private IMapper _mapper;
        private CastVoteCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<LoveAtFirstBiteDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // ensure isolation between test runs
                .Options;

            _context = new LoveAtFirstBiteDbContext(options);

            _voteRepo = new GenericRepository<Vote>(_context);
            _userRepo = new GenericRepository<User>(_context);
            _restaurantRepo = new GenericRepository<Restaurant>(_context);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<VotingProfile>();
            });

            _mapper = config.CreateMapper();

            _handler = new CastVoteCommandHandler(_voteRepo, _userRepo, _restaurantRepo, _mapper, null!);

            // Seed test data
            _context.Users.Add(new User
            {
                UserId = 1,
                UserName = "testuser",
                PasswordHash = "test" // Required property
            });

            _context.Restaurants.Add(new Restaurant
            {
                RestaurantId = 1,
                RestaurantName = "Pizza Place",
                Address = "Restaurant address test"
            });

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Handle_NewVote_ReturnsSuccess()
        {
            // Arrange
            var command = new CastVoteCommand(restaurantId: 1, userId: 1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(1, result.Data.RestaurantId);
        }

        [Test]
        public async Task Handle_DuplicateVote_ReturnsFailure()
        {
            // Arrange
            var existingVote = new Vote
            {
                UserId = 1,
                RestaurantId = 1,
                VoteDate = DateTime.UtcNow.Date,
                Round = 1
            };
            _context.Votes.Add(existingVote);
            _context.SaveChanges();

            var command = new CastVoteCommand(restaurantId: 1, userId: 1, round: 1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.That(result.Errors, Contains.Item("You’ve already voted for this restaurant this round."));
        }
    }
}
