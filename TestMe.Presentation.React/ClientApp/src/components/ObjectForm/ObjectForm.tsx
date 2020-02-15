import * as React from 'react';
import { ObjectFormItem } from './ObjectFormItem';
import { ObjectUtils } from '../../utils';

type ObjectWithIndexer = object & { [key: string]: any };

export class ObjectForm
{
    public static applyChangesAndResetErrors<T extends ObjectWithIndexer>(form: T): T 
    {  
        const formCopy = ObjectUtils.deepClone(form);

        for (const [key, value] of Object.entries(formCopy))
        {
            if (Array.isArray(value))
            {
                const array = value as ObjectFormItem[];
                for (const item of array)
                {
                    this.resetItem(item);
                }
            }
            else
            {
                this.resetItem(value);
            }
        }

        return formCopy;
    }
    public static areItemsEqual(left: ObjectFormItem, right: ObjectFormItem): boolean
    {
        return (left.value === right.value) && (left.error === right.error);
    }
    public static hasValidationErrors<T extends ObjectWithIndexer>(formData: T)
    {       
        for (const [key, value] of Object.entries(formData))
        {
            if (Array.isArray(value))
            {
                const array = value as ObjectFormItem[];
                for (const item of array)
                {
                    if (item.error != null)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (value.error != null)
                {
                    return true;
                }
            }
        }
        return false;
    }


    public static mapToDTO<Tfrom extends ObjectWithIndexer, Tto extends ObjectWithIndexer>(form: Tfrom, dto: ObjectWithIndexer): Tto
    {       
        for (const [key, value] of Object.entries(form))
        {
            if (dto.hasOwnProperty(key))
            {
                dto[key] = value.value;
            }
            else
            {
                throw Error(`ObjectForm error, property ${key} does not exist on object ${dto.toSource()}`);
            }
        }
        return dto as Tto;
    }
    public static mapToForm<Tfrom extends ObjectWithIndexer, Tto extends ObjectWithIndexer>(dto: Tfrom, form: Tto): Tto
    {
        for (const [key, value] of Object.entries(form))
        {
            if (dto.hasOwnProperty(key))
            {
                form[key].value = dto[key];
            }
            else
            {
                throw Error(`ObjectForm error, property ${key} does not exist on object ${dto.toSource()}`);
            }
        }
        return form;
    }


    private static resetItem(item: ObjectFormItem)
    {
        item.error = null;
        if (item.newValue !== undefined)
        {
            item.value = item.newValue;
            item.newValue = undefined;
        }
    }
}