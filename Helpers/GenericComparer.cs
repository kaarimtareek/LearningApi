using ObjectsComparer;

namespace Helpers
{
    public static class GenericComparer
    {
        public static bool Compare<T>(T object1, T object2)
        {
            if (object1 == null || object2 == null)
            {
                return false;
            }
            var comparer = new Comparer();
            bool isEqual = comparer.Compare<T>(object1, object2);
            return isEqual;
        }
        //    foreach(var property in properties)
        //    {
        //        string objectValue1 = string.Empty;
        //        string objectValue2 = string.Empty;
        //      var typeValue1=  type.GetProperty(property.Name).GetValue(object1);
        //        var typeValue2 = type.GetProperty(property.Name).GetValue(object2);
        //        if (typeValue1 == null || typeValue2 == null)
        //            return false;
        //        if (typeValue1.ToString().Trim() != typeValue2.ToString().Trim())
        //            return false;

        //    }
        //    return true;
        //}
    }
}
