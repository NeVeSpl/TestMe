export class StateStorage<T>
{
    keyName: string;

    constructor(private typeConstructor: new () => T)
    {
        this.keyName = typeConstructor.name;
    }


    Load() : T | null
    {
        const serializedObject = localStorage.getItem(this.keyName);
        if (serializedObject != null)
        {
            const object = JSON.parse(serializedObject);
            return object;
        }
        return new this.typeConstructor();
    }
    Save(object: T)
    {
        const serializedObject = JSON.stringify(object);        
        localStorage.setItem(this.keyName, serializedObject);
    }
    Erase()
    {
        localStorage.removeItem(this.keyName);
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