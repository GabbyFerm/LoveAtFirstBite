using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Restaurant.Commands
{
    public class DeleteRestaurantCommand : IRequest<OperationResult<bool>>
    {
        public int Id { get; }

        public DeleteRestaurantCommand(int id)
        {
            Id = id;
        }
    }

}
