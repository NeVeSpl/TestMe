import * as React from 'react';
import { render } from '@testing-library/react';
import Question, { QuestionState } from './Question';
import { StateStorage } from '../../../../utils/StateStorage';

test('renders Question', () =>
{
    const storage = StateStorage.CreateMock<QuestionState>();
    const result = render(<Question injectedStorage={storage} />);   
});
