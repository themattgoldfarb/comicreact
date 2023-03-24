import React, { Component } from 'react';
import Card from 'react-bootstrap/Card';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { Link } from 'react-router-dom';
import './Library.css';

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

  static renderLibrary(comicLibrary) {
    return (
      <Container fluid className="library-container wrap">
        {comicLibrary.map(comic =>
          <div key={comic.title}>
            <Link to={"/reader/"+comic.title}>
              <Card 
                  className="bubble-text library-card"
                  bg="dark"
                  text="white">
                <Card.Img src={"comic/page/"+comic.title+"/0"}/>
                <Card.Body>
                  <Card.Title> {comic.title} </Card.Title>
                  <Card.Text> {comic.description} </Card.Text>
                </Card.Body>
              </Card>
            </Link>
          </div>
          )}
        </Container>
    );
  }

  render() {
    let contents = this.state.loading
      ? Library.renderLoading()
      : Library.renderLibrary(this.state.comicLibrary);

    return (
      <div>
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
