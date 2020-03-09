export interface ProblemDetails 
{
    title: string;
    status: number;  
    detail: string;    
    traceId: string;
    errors: object & { [key: string]: string[] }
}