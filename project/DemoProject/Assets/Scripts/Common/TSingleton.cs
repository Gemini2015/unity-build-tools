namespace CC.Common
{
    public class TSingleton<T> where T: TSingleton<T>
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = System.Activator.CreateInstance<T>();
                    instance.OnCreateInstance();
                }

                return instance;
            }
        }

        public static bool HasInstance()
        {
            return instance != null;
        }

        public static void TryResetInstance()
        {
            if(instance != null)
            {
                instance.OnResetInstance();
                instance = default(T);
            }
        }

        protected virtual void OnCreateInstance()
        {
        }

        protected virtual void OnResetInstance()
        {

        }
    }

}
