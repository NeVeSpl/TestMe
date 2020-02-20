import * as React from 'react';
import { render } from '@testing-library/react';
import { Page404 } from './Page404';
import { RouteComponentProps } from 'react-router-dom';

test('renders Page404', () =>
{
    const result = render(<Page404  {...({} as RouteComponentProps)}/>);   
});
