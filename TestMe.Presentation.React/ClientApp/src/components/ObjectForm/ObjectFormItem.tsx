import * as React from 'react';

export class ObjectFormItem
{    
    value: string;
    isTouched: boolean;
    error: string | null;
    newValue: string | undefined;

    constructor()
    {
        this.value = "";
        this.isTouched = false;
        this.error = null;        
    }
}