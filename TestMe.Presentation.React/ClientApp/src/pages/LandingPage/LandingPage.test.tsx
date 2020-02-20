import * as React from 'react';
import { render } from '@testing-library/react';
import { LandingPage } from './LandingPage';
import { RouteComponentProps } from 'react-router-dom';

test('renders LandingPage', () =>
{
    const result = render(<LandingPage {...({} as RouteComponentProps)}/>);   
});
