namespace Core.Service.Base.DataSets
{
    public abstract class BaseModel<T> where T : struct, IComparable, IFormattable, IConvertible {

        public Guid Id { get; set; } = Guid.NewGuid();
        
        public int Order { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public T State { get; set; }

    }

}
