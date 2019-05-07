import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';

export class Page404 extends React.Component<RouteComponentProps>
{
    render()
    {
        return (
            <h1>There is no route you are looking for.</h1>
        );
    }
}
