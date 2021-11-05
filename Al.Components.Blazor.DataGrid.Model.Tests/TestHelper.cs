using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace Al.Components.Blazor.DataGrid.Model.Tests
{
    public static class TestHelper
    {
        /// <summary>
        /// Проверяет правильность работы свойства с "асинхронным сеттером"
        /// </summary>
        /// <typeparam name="T">Тип записи</typeparam>
        /// <typeparam name="TProp">Тип поля экземпляра записи</typeparam>
        /// <param name="instance">Экземпляр записи</param>
        /// <param name="propertyName">имя свойства</param>
        /// <param name="defaultValue">значение, которое должно быть установлено по-умолчанию для свойства</param>
        /// <param name="notDefaultValue">Значение не по-умолчанию, которое будет установлено для проверки сеттера</param>
        /// <param name="setterName">имя метода асинхронного сеттера</param>
        /// <param name="eventName">имя события, которое должно сработать при установке сеттера, если есть</param>
        /// <param name="expectedNotDefaultValue"></param>
        public static void TestAsynSetter<T, TProp>(T instance, string propertyName,
            TProp defaultValue, TProp notDefaultValue, string setterName,
            string eventName, TProp expectedNotDefaultValue)
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

            EventInfo? even = null;
            bool callEvent = false;

            if (eventName != null)
            {
                even = _type.GetEvent(eventName);
                even.AddEventHandler(instance, EventHandler);
            }


            //act
            var result = (Task)setterMethod.Invoke(instance, new object?[] { notDefaultValue });
            result.Wait();


            //assert
            var propNewValue = property.GetValue(instance);
            Assert.Equal(expectedNotDefaultValue, propNewValue);

            if(eventName != null)
                Assert.True(callEvent);


            even.RemoveEventHandler(instance, EventHandler);

            async Task EventHandler()
            {
                callEvent = true;
            }
        }


    }
}
