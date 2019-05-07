
export class ArrayUtils
{
    static ReplaceFirst<T>(array: Array<T>, predicate: (obj : T) => boolean, newObj : T) : Array<T>
    {
        const newArray = [...array];
        const index = newArray.findIndex(predicate);
        if (index > -1)
        {
            newArray[index] = newObj;
        }
        return newArray;
    }
}