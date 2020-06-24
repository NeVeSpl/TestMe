import * as React from 'react';
import { RouteProps } from 'react-router';
import { useSelector, useDispatch } from 'react-redux'
import { RootState } from '../../../redux.base';
import { BusyIndicator, Window } from '../../../components';
import { fetchCatalogs } from './QuestionsCatalogs.reducer';
import { useEffect } from 'react';
import { preventDefault } from '../../../utils/ReactUtils';
import { ShowQuestionsCatalog } from '../QuestionsCatalog/QuestionsCatalog.reducer';
import { ShowQuestionsCatalogEditor } from '../QuestionsCatalogEditor/QuestionsCatalogEditor.reducer';

interface QuestionCatalogsProps extends RouteProps
{
    
}

export function QuestionsCatalogs(props: QuestionCatalogsProps)
{
    const { apiServiceState, openedChildWindowCounter, questionsCatalogs } = useSelector((state: RootState) => state.questionsCatalogs);  
    const dispatch = useDispatch()
    // eslint-disable-next-line react-hooks/exhaustive-deps
    useEffect(() => { dispatch(fetchCatalogs()) }, []);   

    return (
        <>
            <Window title="Catalogs of questions" error={apiServiceState.apiError} isEnabled={openedChildWindowCounter === 0} >
                <BusyIndicator isBusy={apiServiceState.isBusy}>
                    {() => 
                        <div className="list-group">
                            {questionsCatalogs.sort((a, b) => a.name.localeCompare(b.name)).map(x =>
                                <a className="list-group-item list-group-item-action" href="#" key={x.catalogId} onClick={preventDefault(() => dispatch(new ShowQuestionsCatalog(x.catalogId)))}>{x.name}</a>
                            )}
                            <button className="list-group-item list-group-item-action list-group-item-primary text-center" onClick={preventDefault(() => dispatch(new ShowQuestionsCatalogEditor()))}>Add new catalog</button>
                        </div>
                    }
                </BusyIndicator>
            </Window>           
        </>
    );
}