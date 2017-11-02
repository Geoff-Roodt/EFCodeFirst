namespace ToDoApp
{
    public static class CommonLogic
    {
        public static Java.Lang.Object ToJavaObject<T>(this T value)
        {
            if (Equals(value, default(T)))
                return null;

            var holder = new JavaHolder(value);
            return holder;
        }
    }

    internal class JavaHolder : Java.Lang.Object
    {
        public readonly object Instance;

        public JavaHolder(object instance)
        {
            Instance = instance;
        }
    }
}