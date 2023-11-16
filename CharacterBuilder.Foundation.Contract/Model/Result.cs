#nullable disable
using System;

namespace CharacterBuilder.Foundation.Model
{
    public class Result<T>
    {
        public T Value { get; init; }
        public bool HasValue { get; set; }
        public string Message { get; set; }

        public Result()
        {

        }

        public Result(T value)
        {
            Value = value;
            HasValue = true;
            Message = null;
        }

        public Result(Exception e)
        {
            Value = default;
            HasValue = false;
            Message = e.Message;
        }
    }
}
