using System;
using System.Reflection;
using AutoFixture.Kernel;
using InventoryСontrol.Domain;

namespace InventoryСontrol.Tests
{
    public class PersonExternalIdGenerator : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is PropertyInfo pi)
                if (pi.Name == nameof(User.Id))
                    return Guid.NewGuid().ToString();

            return new NoSpecimen();
        }
    }
}