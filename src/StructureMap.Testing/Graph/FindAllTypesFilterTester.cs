﻿using System;
using Moq;
using NUnit.Framework;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace StructureMap.Testing.Graph
{
    public class FindAllTypesFilterTester
    {
        [Test]
        public void it_registers_types_that_can_be_cast()
        {
            var registry = new Mock<Registry>(MockBehavior.Strict);
            registry.Expect(x => x.AddType(It.IsAny<Type>(), typeof (Generic<>), It.IsAny<string>()))
                .Callback<Type, Type, string>((x, y, z) =>
                                              Assert.That(x.GetGenericTypeDefinition(), Is.EqualTo(typeof (IGeneric<>))));
            var filter = new FindAllTypesFilter(typeof (IGeneric<>));

            filter.Process(typeof (Generic<>), registry.Object);
            registry.VerifyAll();
        }

        [Test]
        public void it_registers_types_implement_the_closed_generic_version()
        {
            var registry = new Mock<Registry>(MockBehavior.Strict);
            registry.Expect(x =>
                            x.AddType(typeof (IGeneric<string>), typeof (StringGeneric), It.IsAny<string>()));
            var filter = new FindAllTypesFilter(typeof (IGeneric<>));

            filter.Process(typeof (StringGeneric), registry.Object);
            registry.VerifyAll();
        }

        #region Nested type: Generic

        public class Generic<T> : IGeneric<T>
        {
            public void Nop()
            {
            }
        }

        #endregion

        #region Nested type: IGeneric

        public interface IGeneric<T>
        {
            void Nop();
        }

        #endregion

        #region Nested type: StringGeneric

        public class StringGeneric : Generic<string>
        {
        }

        #endregion
    }
}