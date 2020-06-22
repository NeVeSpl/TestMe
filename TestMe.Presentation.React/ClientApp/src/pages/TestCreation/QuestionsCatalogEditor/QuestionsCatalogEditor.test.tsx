import * as React from 'react';
import { render } from '@testing-library/react';
import QuestionsCatalogEditor, { QuestionsCatalogEditorState } from './QuestionsCatalogEditor';
import { StateStorage } from '../../../utils/StateStorage';

test('renders QuestionsCatalogEditor', () =>
{
    const storage = StateStorage.CreateMock<QuestionsCatalogEditorState>();
    const result = render(<QuestionsCatalogEditor injectedStorage={storage} />);   
});
