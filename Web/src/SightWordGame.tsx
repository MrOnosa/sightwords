import * as React from 'react';

declare var process : {
  env: {
    NODE_ENV: string
  }
}

interface SightWord {
  id: number,
  word: string
}

interface LessonSet
{
  words: SightWord[]
}

interface AnswerDTO
{
    sightwordId: number,
    correct: boolean,
    persistResult: boolean
}

interface SightwordAnswersSummaryDTO
{
    answeredCorrectly: number,
    totalAnswers: number
}

interface Answer
{
  word: string,
  correct: boolean,
  answeredCorrectly?: number,
  totalAnswers?: number
}

interface SightWordGameState {
  wordQueue?: SightWord[],
  answers?: Answer[],
  fetchingWords?: boolean,
  persistAnswers?: boolean
}

export default class SightWordGame extends React.PureComponent<{}, SightWordGameState> {
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

  state: SightWordGameState = {
    wordQueue: [{ word: "Fetching...", id: 0 }],
    answers: [],
    fetchingWords: true,
    persistAnswers: process.env.NODE_ENV === 'production'
  };

  componentDidMount() {
    this.pickWord();
  }

  pickWord = () => {
    if (this.state.wordQueue.length > 1) {
      this.setState((prevState) => {
        const tail = prevState.wordQueue.slice(1);
        return {
          wordQueue: tail
        }
      });
    }
    else {
      this.setState({
        fetchingWords: true
      })

      fetch('api/sightwords')
        .then((response) => {
          this.setState({
            fetchingWords: false
          });
          if (response.ok) {
            return response.json();
          }
          throw new Error('Network request was not ok.');
        })
        .then((lessonSet: LessonSet) => {
          this.setState({
            wordQueue: lessonSet.words.map((w) => {
              return { ...w, word: w.word.toLocaleLowerCase() };
            })
          });

          if (process.env.NODE_ENV === 'development')
          {
            console.log(JSON.stringify(lessonSet));
          }
        })
        .catch((error) => {
          //If there is a network error, report it to the console, but then use the local dictionary of sightwords
          console.log('There has been a problem with your fetch operation: ', error.message);
          var word = [{ word: this.words[Math.floor(Math.random() * this.words.length)].toLocaleLowerCase() + '.', id: 0 }];

          this.setState({
            wordQueue: word
          });
        });
    }
  }

  answer = (sightword: SightWord, correct: boolean) => {
    const newAnswer:Answer = {
      word: sightword.word,
      correct: correct
    }
    this.setState((prevState) => {
      return {
      answers: [newAnswer, ...prevState.answers]
    }});

    const answer:AnswerDTO = {
      sightwordId: sightword.id,
      correct: correct,
      persistResult: this.state.persistAnswers 
    };

    fetch('api/sightwords/answer', {
      method: 'PUT',
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(answer)
    })
    .then(response => {
      if (!response.ok) {
        throw new Error('Network request was not ok.');
      }
      return response.json()
    })
    .then((summary: SightwordAnswersSummaryDTO) => {
      this.setState((prevState) => {
        return {
        answers: prevState.answers.map((a) => {
          if(a === newAnswer)
            return {...a, answeredCorrectly: summary.answeredCorrectly, totalAnswers: summary.totalAnswers  }
          return a;
          })
        }
      });

      if (process.env.NODE_ENV === 'development')
      {
        console.log(JSON.stringify(summary));
      }
    })
    .catch((error) => {
      //If there is a network error, report it to the console
      console.log('There has been a problem when submitting your answer: ', error.message);
    });
  }

  correct = (sightword: SightWord) => {
    if(!this.state.fetchingWords)
    {
      this.answer(sightword, true);
      this.pickWord();
    }
  };

  incorrect = (sightword: SightWord) => {
    if(!this.state.fetchingWords)
    {
      this.answer(sightword, false);
      this.pickWord();
    }
  };

  handleInputChange = (event) => {
    const target = event.target;
    const value = target.type === 'checkbox' ? target.checked : target.value;
    const name = target.name;

    this.setState({
      [name]: value
    });
  }

  render() {
    const [sightword] = this.state.wordQueue;
    const answerRows = this.state.answers.map((answer, i) => {
      return (
        <tr key={i}>
          <td>{answer.word}</td>
          <td>{answer.correct ? "Correct" : "Wrong"}</td>
          <td>{(answer.answeredCorrectly !== undefined ? answer.answeredCorrectly : "-") + " out of "}
          {(answer.totalAnswers !== undefined ? answer.totalAnswers : "-")}
          <br/>
          {!!answer.totalAnswers ? Math.round(100 * (answer.answeredCorrectly/answer.totalAnswers)) : "---"}%
          </td>
        </tr>
      );
    })
    const answerTable = this.state.answers.length > 0 ? (<div>
      <table align="center">
        <thead>
          <tr>
            <th colSpan={3}>
              Answers
            </th>                
          </tr>
          <tr>
            <th>
              Word
            </th>
            <th>
              Result
            </th>         
            <th>
              Record
            </th>       
          </tr>
        </thead>
        <tbody>
          {answerRows}
        </tbody>
      </table>
    </div>) : null;
    return (
      <div style={{ textAlign: "center" }}>
        <h1>{sightword.word}</h1>
        <button onClick={() => this.correct(sightword)} style={{ "color": "green", marginRight: "5px" }}>Correct</button>
        <button onClick={() => this.incorrect(sightword)} style={{ "color": "red" }}>Wrong</button>
        <hr/>
        <div>
          <label>Save Results:
            <input 
              name="persistAnswers"
              type="checkbox" 
              checked={this.state.persistAnswers}
              onChange={this.handleInputChange} />
          </label>
        </div>
        {answerTable}
      </div>
    );
  }
}