import * as React from 'react';
import { render } from '@testing-library/react';
import QuestionEditor, { QuestionEditorState } from './QuestionEditor';
import { StateStorage } from '../../../../utils/StateStorage';

test('renders QuestionEditor', () =>
{
    const storage = StateStorage.CreateMock(QuestionEditorState);
    const result = render(<QuestionEditor injectedStorage={storage}/>);   
});
