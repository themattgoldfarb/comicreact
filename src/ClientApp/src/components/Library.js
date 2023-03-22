import React, { Component } from 'react';
import Card from 'react-bootstrap/Card';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
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

  static renderComic(comic) {
    <li>
      Title: {comic.Title}
      Description: {comic.Description}
    </li>
  }

  static renderLibrary(comicLibrary) {
    return (
      <Container fluid style={{ gap: '10px' }}>
        <Row>
          {comicLibrary.map(comic =>
          <Col style={{ gap: '10px' }}>
            <Card 
              className="bubble-text"
              bg="dark"
              text="white"
              style={{ gap: '10px', height: '18rem', width: '12rem' }}>
            <Card.Img src={"comic/page/"+comic.title+"/0"}/>
            <Card.ImgOverlay>
              <Card.Text> {comic.title} </Card.Text>
              <Card.Text
                style={{position: 'absolute', bottom: 0}}
              > {comic.description} </Card.Text>
            </Card.ImgOverlay>
          </Card>
          </Col>
          )}
        </Row>
      </Container>
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
