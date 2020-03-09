export interface CursorPagedResults<T>
{
    cursor: number;
    nextCursor: number;
    result: T[];
}