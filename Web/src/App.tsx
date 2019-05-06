import * as React from 'react';
import { render } from 'react-dom';
import WebFont from 'webfontloader';
import SightWordGame from './SightWordGame';
import './index.css';

render(<SightWordGame />, document.getElementById('main'));

WebFont.load({
    google: {
        families: ['Roboto', 'sans-serif']
    }
});