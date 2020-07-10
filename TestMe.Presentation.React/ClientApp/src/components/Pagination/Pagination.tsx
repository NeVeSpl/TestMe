import * as React from 'react';
import { useState } from "react";
import { preventDefault } from '../../utils/ReactUtils';
import styles from './Pagination.module.css';



interface PaginationProp
{
    onShiftLeft?: (newPage : number) => void;
    onShiftRight?: (newPage: number) => void;
    canShiftRight: boolean;
}


export function Pagination(props: PaginationProp)
{
    const [page, setPage] = useState(1);
    const handleShiftLeft = () =>
    {
        if (page > 1)
        {
            setPage(page - 1);
            props.onShiftLeft?.(page - 1);
        }
    };
    const handleShiftRight = () =>
    {
        if (props.canShiftRight)
        {
            setPage(page + 1);
            props.onShiftRight?.(page + 1);
        }
    };


    return (
        <nav >
            <ul className={styles.pagination + " pagination"} >
                <li className={"page-item" + (page > 1 ? "" : " disabled")}>
                    <a className="page-link" href="#" onClick={preventDefault(handleShiftLeft)} aria-label="Previous">
                        <span aria-hidden="true">&laquo;</span>
                        <span className="sr-only">Previous</span>
                    </a>
                </li>
                <li className="page-item"><a className="page-link" href="#" onClick={preventDefault(() => { })}>{page}</a></li>
                <li className={"page-item" + (props.canShiftRight ? "" : " disabled")}>
                    <a className="page-link" href="#" onClick={preventDefault(handleShiftRight)} aria-label="Next">
                        <span aria-hidden="true">&raquo;</span>
                        <span className="sr-only">Next</span>
                    </a>
                </li>
            </ul>
        </nav>
        );
}