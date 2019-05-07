import './App.css';
import React, { Component } from 'react';
import { Route, Switch } from 'react-router';
import { LandingPage, TestCreationPage, Page404 } from './pages';
import { Layout, AuthenticatedRoute } from './components';

export default class App extends Component
{
    render()
    {
        return (
            <Layout>
                <Switch>
                    <Route exact path='/' component={LandingPage} />
                    <AuthenticatedRoute path='/testcreation' fallbackPath="/" component={TestCreationPage} />
                    <Route component={Page404} />
                </Switch>
            </Layout>
        );
    }
}
