using System.ComponentModel;

namespace LanterneRouge.Fresno.Utils.Converters
{
    // https://stackoverflow.com/questions/8625/generic-type-conversion-from-string
    public static class TConverter
    {
        public static T ChangeType<T>(object value) => (T)ChangeType(typeof(T), value);

        public static object ChangeType(Type t, object value)
        {
            var tc = TypeDescriptor.GetConverter(t);
            return tc.ConvertFrom(value);
        }

        public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
        {
            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }
    }
}
