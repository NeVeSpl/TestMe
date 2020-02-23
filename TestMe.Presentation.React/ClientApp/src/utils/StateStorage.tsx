export class StateStorage<T>
{
    private keyName: string

    constructor(private typeConstructor: new () => T, keyName?: string)
    {
        this.keyName = keyName ?? typeConstructor.name;
    }


    Load(): T | null
    {
        try
        {
            const serializedObject = localStorage.getItem(this.keyName);
            if (serializedObject != null)
            {
                const object = JSON.parse(serializedObject);
                return object;
            }
        }
        catch (err)
        {
            console.error(err.message);
        }
        return new this.typeConstructor();
    }
    Save(object: T)
    {
        try
        {
            const serializedObject = JSON.stringify(object);
            localStorage.setItem(this.keyName, serializedObject);
        }
        catch (err)
        {
            console.error(err.message);
        }
    }
    Erase()
    {
        try
        {
            localStorage.removeItem(this.keyName);
        }
        catch (err)
        {
            console.error(err.message);
        }
    }


    static CreateMock<T>(typeConstructor: new () => T): StateStorage<T>
    {
        const storage = new StateStorage(typeConstructor);
        storage.Load = () => null;
        storage.Save = () => null;
        storage.Erase = () => null;
        return storage;
    }
}