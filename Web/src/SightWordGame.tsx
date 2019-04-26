import * as React from 'react';

export default class SightWordGame extends React.Component {
  readonly words = [
      "And",
      "Said",      
      "To",

      "For",
      "Like",
      "No",
      
      "Of",
      "Off",
      "See",
      
      "Are",
      "Not",      
      "Or",
      
      "Have",
      "On",
      "My",
      
      "By",
      "Good",
      "Out",
      
      "Put",
      "You",
      "Can",
      
      "Look",
      "With",
      "We",
      
      "This",
      "Is",
      "I",
      
      "Me",
      "She",
      "We",
      
      "He",
      "Be",
      "Has",
      
      "Saw",
      "The",
      "Her",
      
      "Then",
      "An",
      "Was",
      
      "As",
      "Go",
      "Want",
      
      "Went",
      "A",
      "Here",
      
      "There",
      "Am",
      "Come",
      
      "Home",
      "That",
      "They",
      "What",
  ];

  state = {
    sightWord: "Fetching..."
  };

  componentDidMount(){
    this.pickWord();
  }

  pickWord = () => {
    fetch('api/sightwords')
    .then((response) => {
      if(response.ok){
        return response.json();
      }
      throw new Error('Network request was not ok.');      
    })
    .then((myJson) => {
      this.setState({
          sightWord: (myJson.word1.toLocaleLowerCase())
      });
      console.log(JSON.stringify(myJson));
    })
    .catch((error) => {
      //If there is a network error, report it to the console, but then use the local dictionary of sightwords
      console.log('There has been a problem with your fetch operation: ', error.message);
      var word = this.words[Math.floor(Math.random()*this.words.length)].toLocaleLowerCase() + '.';
      
      this.setState({
        sightWord: (word)
      });
    });;
    //return this.words[Math.floor(Math.random()*this.words.length)].toLocaleLowerCase();
  }

  increment = () => {
    this.pickWord();
  };

  decrement = () => {
    this.pickWord();
  };

  render () {
    return (
      <div style={{textAlign: "center"}}>          
        <h1>{this.state.sightWord}</h1>
        <button onClick={this.increment} style={{"color": "green", marginRight: "5px"}}>Correct</button>
        <button onClick={this.decrement} style={{"color": "red"}}>Wrong</button>
      </div>
    );
  }
}