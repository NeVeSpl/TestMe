import * as React from 'react';
import { render } from '@testing-library/react';
import { BusyIndicator } from './BusyIndicator';

test('renders BusyIndicator', () =>
{
    const result = render(<BusyIndicator isBusy={true} >{() => { }}</BusyIndicator>);   
});
