import {ErrorCode, ProblemDetails } from '.';


export class ApiError extends Error
{
    readonly errorCode: ErrorCode;
    readonly traceId: string | undefined;
    readonly detail: string | undefined;


    constructor(errorCode: ErrorCode, problem?: ProblemDetails, ...params: any[]) 
    {       
        super(...params);

        // Maintains proper stack trace for where our error was thrown (only available on V8)
        if (Error.captureStackTrace) 
        {
            Error.captureStackTrace(this, ApiError);
        }

        this.errorCode = errorCode;
        if (problem !== undefined)
        {
            this.traceId = problem!.traceId;
            this.detail = problem!.detail;

            if (this.errorCode === ErrorCode.Conflict)
            {
                this.detail = "Someone has done changes before you. The conflict was automatically resolved. Please review changes and redo operation.";
            }
        }
    }
}