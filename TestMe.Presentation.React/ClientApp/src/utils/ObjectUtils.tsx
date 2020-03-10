
export class ObjectUtils
{
    static deepClone<T>(object: T): T
    {
        const serializedObject = JSON.stringify(object);
        const copy = JSON.parse(serializedObject);
        return copy;
    }



    /// source : https://github.com/reduxjs/redux/blob/master/src/utils/isPlainObject.ts
    static isPlainObject(obj: any): boolean
    {
        if (typeof obj !== 'object' || obj === null) return false

        let proto = obj
        while (Object.getPrototypeOf(proto) !== null)
        {
            proto = Object.getPrototypeOf(proto)
        }

        return Object.getPrototypeOf(obj) === proto
    }
}