﻿using Microsoft.AspNetCore.Mvc;

namespace SharedLibrary.DTO.Result;

/// <summary>
///  Custom response dto  with factory
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class CustomResponseDto<TEntity>
{
    public bool IsSuccess { get; private set; }
    public TEntity? Data { get; private set; }
    public int StatusCode { get; private set; }
    public ProblemDetails? ProblemDetails { get; private set; }

    private CustomResponseDto(bool isSuccess, TEntity? data, int statusCode, ProblemDetails? problemDetails)
    {
        IsSuccess = isSuccess;
        Data = data;
        StatusCode = statusCode;
        ProblemDetails = problemDetails;
    }

    public static CustomResponseDto<TEntity> Success(TEntity data, int statusCode)
    {
        return new CustomResponseDto<TEntity>(true, data, statusCode, default);
    }

    public static CustomResponseDto<TEntity> Success(int statusCode)
    {
        return new CustomResponseDto<TEntity>(true, default, statusCode, default);
    }

    // AKA fail
    public static CustomResponseDto<TEntity> Exception(ProblemDetails problemDetails)
    {
        return new CustomResponseDto<TEntity>(false, default, (int)problemDetails.Status, problemDetails);
    }
    
    public static CustomResponseDto<TEntity> Fail(string errorMessage,int statusCode,string detail)
    {
        var problemDetails = new ProblemDetails()
        {
            Status = statusCode,
            Title = errorMessage,
            Detail = detail,
        };
        return new CustomResponseDto<TEntity>(false, default,statusCode, problemDetails);
    }
}
