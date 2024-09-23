import React from "react";
import Input from "../input/Input";
import { ILoginField } from "../../configs/form-config";

interface IInputFormProps {
    configurations: ILoginField[];
}

const InputForm = (props: IInputFormProps): JSX.Element => {
    const getElement = (props: ILoginField): JSX.Element => {
        switch (props.type) {
            // Fall-through
            case 'email':
            case 'text':
            case 'password':
                return (
                    <>
                        <Input defaultText={props.defaultText}
                            onHandleInputChange={props.onChange}
                            value={props.value}
                            type={props.type}
                            name={props.name} />
                        <br />
                    </>
                )

            default:
                console.log('No element was found!');
        }
    }

    return (
        <div>
            {props.configurations.map(configuration => {
                return getElement(configuration);
            })}
        </div>
    )
}

export default InputForm;