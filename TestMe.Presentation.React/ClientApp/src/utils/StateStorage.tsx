export class StateStorage<T>
{
    keyName: string;

    constructor(private typeConstructor: new () => T)
    {
        this.keyName = typeConstructor.name;
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
        finally
        {

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
        finally
        {

        }
    }
    Erase()
    {
        try
        {
            localStorage.removeItem(this.keyName);
        }
        finally
        {

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