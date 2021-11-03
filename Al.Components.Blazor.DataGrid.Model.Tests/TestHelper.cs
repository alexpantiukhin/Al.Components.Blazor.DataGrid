using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests
{
    public static class TestHelper
    {

        public static void TestAsynSetter<T, TProp>(T instance, string propertyName, TProp defaultValue, TProp notDefaultValue, string setterName, string eventName)
            where T : class
        {
            //arrange
            Type _type = typeof(T);
            var property = _type.GetProperty(propertyName);


            //act
            TProp propValue = (TProp)property.GetValue(instance);


            //assert
            Assert.Equal(defaultValue, propValue);


            //arrange
            var setterMethod = _type.GetMethod(setterName);
            var even = _type.GetEvent(eventName);
            bool callEvent = false;
            even.AddEventHandler(instance, EventHandler);


            //act
            var result = (Task)setterMethod.Invoke(instance, new object?[] { notDefaultValue });
            result.Wait();


            //assert
            var propNewValue = property.GetValue(instance);
            Assert.Equal(notDefaultValue, propNewValue);
            Assert.True(callEvent);


            async Task EventHandler()
            {
                callEvent = true;
            }
        }


    }
}
