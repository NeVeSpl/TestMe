import * as React from 'react';
import { MagicDict } from './MagicDict'
import { ObjectUtils } from '../../utils';

export const MagicFormContext = React.createContext({});

interface MagicFormProps
{
    children: (JSX.Element | JSX.Element[])[];
    onInputChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
    validationErrors?: MagicDict;   
    conflictErrors?: MagicDict;
}

enum ParentType {None, Object, Array}

export class MagicForm extends React.PureComponent<MagicFormProps>
{    
    render()
    {
        const context =
        {
            onInputChange: this.props.onInputChange,
            validationErrors: this.props.validationErrors || {},
            conflictErrors: this.props.conflictErrors || {}
        };

        return (
            <MagicFormContext.Provider value={context}>
                <form>
                    {this.props.children}
                </form>
            </MagicFormContext.Provider>
        );
    }

    public static ResolveConflicts<T extends MagicDict>(originalFormData: MagicDict, currentFormData: T, newFormData: MagicDict): ResolveConflictsResult<T>
    {
        const resolvedFormData = ObjectUtils.deepClone(currentFormData);
        const conflictErrors: MagicDict = {};

        this.ResolveConflictsRecursively(originalFormData, currentFormData, newFormData, resolvedFormData, conflictErrors, "", ParentType.None);

        return new ResolveConflictsResult(conflictErrors, resolvedFormData);
    }
    private static ResolveConflictsRecursively<T extends MagicDict>(originalFormData: MagicDict, currentFormData: T, newFormData: MagicDict, resolvedFormData: T, conflictErrors: MagicDict, path: string, parentType: ParentType)
    {
        let keys = Array.from(new Set([...Object.keys(currentFormData), ...Object.keys(newFormData)]));
        for (let key of keys)
        {            
            const newValue = newFormData[key];
            const oldValue = originalFormData[key];
            const currentValue = currentFormData[key];

            if (Array.isArray(currentValue))
            {
                this.ResolveConflictsRecursively(originalFormData[key], currentFormData[key], newFormData[key], resolvedFormData[key], conflictErrors, this.ExtendPath(path, key, parentType), ParentType.Array);
                continue;
            }
            if (currentValue instanceof Object === true)
            {
                this.ResolveConflictsRecursively(originalFormData[key], currentFormData[key], newFormData[key], resolvedFormData[key], conflictErrors, this.ExtendPath(path, key, parentType), ParentType.Object);
                continue;
            }

            if (newValue !== oldValue)
            {
                if (currentValue !== oldValue)
                {
                    if (newValue !== currentValue)
                    {
                        conflictErrors[this.ExtendPath(path, key, parentType)] = newValue;
                    }
                }
                else
                {
                    resolvedFormData[key] = newValue;
                }
            }
        }
    }
    private static ExtendPath(path: string, newElement: string, parentType: ParentType): string
    {
        switch (parentType)
        {
            case ParentType.None:
                return `${path}${newElement}`                
            case ParentType.Array:
                return `${path}[${newElement}]`
            case ParentType.Object:
                return `${path}.${newElement}`
        }
        throw new RangeError();
    }
    public static ApplyEventTo<T extends MagicDict>(event: React.ChangeEvent<HTMLInputElement>, state: T): T
    {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const path = target.name.replace(/\[(\w+)\]/g, '.$1');
        let newState = MagicForm.CloneObjectOrArray(state);
        MagicForm.ApplyChangeOnPath(path, newState, value);
        return newState;
    }
    private static ApplyChangeOnPath<T extends MagicDict>(path: string, state: T, value: any)
    {
        const tokenEnd = path.indexOf(".");

        if (tokenEnd === -1)
        {
            state[path] = value;
            return;
        }

        const name = path.substring(0, tokenEnd);
        state[name] = MagicForm.CloneObjectOrArray(state[name]);

        MagicForm.ApplyChangeOnPath(path.substring(tokenEnd + 1), state[name], value);
        return;
    }
    private static CloneObjectOrArray<T extends MagicDict>(source: T): T
    {
        const clone = Array.isArray(source) ? [...source] : { ...source };
        return clone as T;
    }
}

export class ResolveConflictsResult<T extends MagicDict>
{
    conflictErrors: MagicDict;
    resolvedFormData: T;

    constructor(conflictErrors: MagicDict, resolvedFormData: T)
    {
        this.conflictErrors = conflictErrors;
        this.resolvedFormData = resolvedFormData;
    }
}