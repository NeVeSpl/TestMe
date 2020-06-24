import React from 'react';
import { NavigationMenu } from './NavigationMenu';


interface LayoutProps
{
    children : (JSX.Element | JSX.Element[]);
}

export function Layout(props: LayoutProps)
{
    return (
        <div>
            <NavigationMenu />
            <div className="container">
                {props.children}
            </div>
        </div>
    );
}