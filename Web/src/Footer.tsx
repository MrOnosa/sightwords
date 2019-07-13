import * as React from 'react';

export default class Footer extends React.PureComponent<{version: string}, {}> {   
    render() {
        return (
            <div className="footer">
                <div>
                    <span>SightWords⭑</span>
                    <a href="https://github.com/MrOnosa/sightwords">Source</a>
                    <span>⭑{this.props.version}</span>
                </div>
            </div>
        );
    }
}