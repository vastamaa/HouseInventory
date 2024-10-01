import React, { ChangeEvent } from 'react'

// Our stuff
import './Input.css'

export interface IInputProps {
    defaultText: string;
    onHandleInputChange: (event: ChangeEvent<HTMLInputElement>) => void;
    value: string;
    name: string;
}

const Input = (props: IInputProps): JSX.Element => {
    const { defaultText, onHandleInputChange, value, name } = props;

    return (
        <div className='input-container'>
            <input
                name={name}
                value={value}
                placeholder={defaultText}
                onChange={onHandleInputChange}
                className='input-box'
            />
        </div>
    )
}

export default Input;