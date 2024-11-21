namespace SharedLibrary.DTO.Result
{
    public class CustomResponseDto<TEntity>
    {
        public bool IsSuccess { get; private set; } 
        public TEntity? Data { get; private set; } 
        public int StatusCode { get; private set; } 
        public List<string>? Errors { get; private set; } 

        private CustomResponseDto(bool isSuccess, TEntity? data, int statusCode, List<string>? errors)
        {
            IsSuccess = isSuccess;
            Data = data;
            StatusCode = statusCode;
            Errors = errors ?? new List<string>();
        }

        public static CustomResponseDto<TEntity> Success(TEntity data, int statusCode)
        {
            return new CustomResponseDto<TEntity>(true, data, statusCode, null);
        }

        public static CustomResponseDto<TEntity> Success(int statusCode)
        {
            return new CustomResponseDto<TEntity>(true, default, statusCode, null);
        }

        public static CustomResponseDto<TEntity> Fail(List<string> errors, int statusCode)
        {
            return new CustomResponseDto<TEntity>(false, default, statusCode, errors);
        }

        public static CustomResponseDto<TEntity> Fail(int statusCode, string error)
        {
            return new CustomResponseDto<TEntity>(false, default, statusCode, new List<string> { error });
        }
    }
}