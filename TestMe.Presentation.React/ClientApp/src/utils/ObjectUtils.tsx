
export class ObjectUtils
{
    static deepClone<T>(object: T): T
    {
        const serializedObject = JSON.stringify(object);
        const copy = JSON.parse(serializedObject);
        return copy;
    }
}