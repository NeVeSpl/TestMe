﻿import * as React from 'react';

export function preventDefault(action: () => any)
{
    return (event: React.MouseEvent<HTMLElement> | null) =>
    {
        if (event != null)
        {
            event.preventDefault();
        }
        action();
    }
}