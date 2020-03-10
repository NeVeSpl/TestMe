import * as React from 'react';
import { render } from '@testing-library/react';
import QuestionsCatalogs, { QuestionsCatalogsState }  from './QuestionsCatalogs';
import { QuestionsCatalogs as QuestionsCatalogsRedux } from './QuestionsCatalogs.redux';
import { StateStorage } from '../../../utils/StateStorage';


test('renders QuestionsCatalogs', () =>
{
    const storage = StateStorage.CreateMock<QuestionsCatalogsState>();
    const result = render(<QuestionsCatalogs injectedStorage={storage} />);   
});
