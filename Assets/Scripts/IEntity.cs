interface IEntity
{
    // Property signatures:
   string apiName
   {
      get;
      set;
   }

   string displayName
   {
      get;
      set;
   }

    void Initialize();
}