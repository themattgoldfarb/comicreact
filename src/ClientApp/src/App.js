import React, { Component } from 'react';
import { Layout } from './components/Layout';
import { Outlet } from 'react-router-dom';
import './custom.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Outlet/>
      </Layout>
    );
  }
}
