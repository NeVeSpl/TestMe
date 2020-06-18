import 'bootstrap/dist/css/bootstrap.css'; 
import 'bootstrap/dist/js/bootstrap.js'; 
import * as React from 'react'; 
import * as ReactDOM from 'react-dom'; 
import { BrowserRouter } from 'react-router-dom'; 
import { Provider } from 'react-redux'
import App from './App'; 
import { configureStore, RootState } from './redux.base';
import { StateStorage } from './utils';
import { ReduxApiFactory } from './autoapi/ReduxApiFactory';


const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') || ""; 
const rootElement = document.getElementById('root'); 

const store = configureStore(new StateStorage<RootState>("ReduxState"), new ReduxApiFactory());

ReactDOM.render(
    <Provider store={store}>
        <BrowserRouter basename={baseUrl}>
            <App />
        </BrowserRouter>
    </Provider>,
    rootElement);