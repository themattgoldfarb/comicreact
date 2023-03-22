import React, { Component } from 'react';

export class Library extends Component {

  constructor (props) {
    super(props);
    this.state = { 
      comicLibrary: [],
      loading: true,
    };
  }

  componentDidMount() {
    this.populateLibrary();
  }

  static renderLoading() {
    return (
      <div>
        <h3>Loading... </h3>
      </div>
    );
  }

  static renderComic(comic) {
    <li>
      Title: {comic.Title}
      Description: {comic.Description}
    </li>
  }

  static renderLibrary(comicLibrary) {
    return (
      <div>
        <h3>My Comics</h3>
        <ul>
          {comicLibrary.map(comic =>
          <li>
             Title: {comic.title} 
            <ul>
              <li>{comic.description} </li>
            </ul>
          </li>
          )}
        </ul>
      </div>
    );
  }

  render() {
    let contents = this.state.loading
      ? Library.renderLoading()
      : Library.renderLibrary(this.state.comicLibrary);

    return (
      <div>
        <h1 id="tabelLabel"> Library</h1>
        <p> This is the library!!!! </p>
        {contents}
      </div>
    );
  }

  async populateLibrary() {
    const response = await fetch('comiclibrary');
    const data = await response.json();
    this.setState({ comicLibrary: data, loading: false });
  }
}
