import { ChangeEvent, MouseEvent } from "react";

interface IFieldBase {
    value: string;
    fieldType: 'input' | 'button';
}

export interface IInputField extends IFieldBase {
    fieldType: 'input';
    name: string;
    defaultText: string;
    onChange: (event: ChangeEvent<HTMLInputElement>) => void; 
}

export interface IButtonField extends IFieldBase { 
    fieldType: 'button';
    onClick: (event: MouseEvent<HTMLInputElement>) => Promise<void>;         
}

export interface IUserLogin {
    email: string;
    password: string;
    rememberMe?: boolean;
}

export interface IUserRegister {
    firstName?: string;
    lastName?: string;
    phoneNumber?: string;
    userName: string;
    email: string;
    password: string;
}

// Union type that can be either an input field or a button
export type Element = IInputField | IButtonField;

interface IGetLoginConfigProps {
    onClick: (event: MouseEvent<HTMLInputElement>) => Promise<void>;
    onChange: (event: ChangeEvent<HTMLInputElement>) => void;
    formDetails: IUserLogin;
}

interface IGetRegisterConfigProps {
    onClick: (event: MouseEvent<HTMLInputElement>) => Promise<void>;
    onChange: (event: ChangeEvent<HTMLInputElement>) => void;
    formDetails: IUserRegister;
}

export const getLoginConfig = (props: IGetLoginConfigProps): Element[] => {
    const { onChange, onClick, formDetails } = props;

    return [
        {
            name: 'email',
            fieldType: 'input',
            defaultText: 'Enter your e-mail here',
            onChange: onChange,
            value: formDetails.email
        },
        {
            name: 'password',
            fieldType: 'input',
            defaultText: 'Enter your password here',
            onChange: onChange,
            value: formDetails.password
        },
        {
            value: 'Log in',
            fieldType: 'button',
            onClick: onClick
        }
    ]
}

export const getRegisterConfig = (props: IGetRegisterConfigProps): Element[] => {
    const { onChange, onClick, formDetails } = props;

    return [
        {
            name: 'firstName',
            fieldType: 'input',
            defaultText: 'Enter your first name here',
            onChange: onChange,
            value: formDetails.firstName ?? ''
        },
        {
            name: 'lastName',
            fieldType: 'input',
            defaultText: 'Enter your last name here',
            onChange: onChange,
            value: formDetails.lastName ?? ''
        },
        {
            name: 'phoneNumber',
            fieldType: 'input',
            defaultText: 'Enter your phone number here',
            onChange: onChange,
            value: formDetails.phoneNumber ?? ''
        },
        {
            name: 'userName',
            fieldType: 'input',
            defaultText: 'Enter your username here',
            onChange: onChange,
            value: formDetails.userName
        },
        {
            name: 'email',
            fieldType: 'input',
            defaultText: 'Enter your e-mail here',
            onChange: onChange,
            value: formDetails.email
        },
        {
            name: 'password',
            fieldType: 'input',
            defaultText: 'Enter your password here',
            onChange: onChange,
            value: formDetails.password
        },
        {
            value: 'Submit',
            fieldType: 'button',
            onClick: onClick
        }
    ]
}