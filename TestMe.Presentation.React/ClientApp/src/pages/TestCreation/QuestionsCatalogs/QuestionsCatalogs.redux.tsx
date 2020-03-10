import * as React from 'react';
import { RouteProps } from 'react-router';
import { useSelector, useDispatch } from 'react-redux'
import { RootState } from '../../../redux.base';
import { BusyIndicator, Window } from '../../../components';
import { ChildWindows, fetchCatalogs, ShowQuestionsCatalog, ShowQuestionsCatalogEditor, CloseWindow } from './QuestionsCatalogs.reducer';
import { useEffect } from 'react';
import { preventDefault } from '../../../utils/ReactUtils';
import { QuestionsCatalog, QuestionsCatalogEditor } from '.';

interface QuestionCatalogsProps extends RouteProps
{
    
}

export function QuestionsCatalogs(props: QuestionCatalogsProps)
{
    const { apiError, isBusy, openedChildWindow, questionsCatalogs, openedQuestionsCatalogId } = useSelector((state: RootState) => state.questionsCatalogs);  
    const dispatch = useDispatch()
    useEffect(() => { dispatch(fetchCatalogs()) }, []);   

    return (
        <>
            <Window title="Catalogs of questions" error={apiError} isEnabled={openedChildWindow === ChildWindows.None} >
                <BusyIndicator isBusy={isBusy}>
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
            {(() =>
                {
                switch (openedChildWindow)
                    {
                        case ChildWindows.QuestionsCatalog:
                            return (
                                <QuestionsCatalog
                                    windowNestingLevel={1}
                                    catalogId={openedQuestionsCatalogId}
                                    onCancel={() => dispatch(new CloseWindow())}
                                    onCatalogDeleted={() => { dispatch(new CloseWindow()); dispatch(fetchCatalogs()) }}
                                    onCatalogUpdated={() => dispatch(fetchCatalogs())}
                                />
                            );
                        case ChildWindows.QuestionsCatalogEditor:
                            return (
                                <QuestionsCatalogEditor
                                    windowNestingLevel={1}
                                    onCancel={() => dispatch(new CloseWindow())}
                                    onCatalogCreated={() => { dispatch(new CloseWindow()); dispatch(fetchCatalogs()) }} />
                            );
                    }
                 })()
            }
        </>
    );
}