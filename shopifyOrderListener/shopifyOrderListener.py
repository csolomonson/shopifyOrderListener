import logging
import os
import time

from imap_tools import A, MailBox, MailMessageFlags
from parser.shopify_parser import parse_shopify


IMAP_HOST = "imap.gmail.com"
EMAIL_ADDRESS = "coledsolomonson@gmail.com"
EMAIL_PASSWORD = "qlis ybnd xqee fvpw"


def handle_email(message) -> None:
    """
    This function runs once for every unprocessed email.
    Replace this body with your real application logic.
    """
    print("New email received")
    print(f"From: {message.from_}")
    print(f"Subject: {message.subject}")
    print(f"Details: {parse_shopify(message)}\n\n")
    print("-" * 50)


def process_unread_messages(mailbox: MailBox) -> None:
    for message in mailbox.fetch(
        A(seen=False,from_='meziere@meziere.com'),
        mark_seen=False,
    ):
        try:
            handle_email(message)
        except Exception:
            # Leave the email unread so it can be retried.
            logging.exception(
                "Failed to process email UID %s",
                message.uid,
            )
        else:
            # Mark it as processed/read only after success.
            mailbox.flag(
                [message.uid],
                (MailMessageFlags.SEEN,),
                True,
            )


def listen_for_email() -> None:
    while True:
        try:
            with MailBox(IMAP_HOST).login(
                EMAIL_ADDRESS,
                EMAIL_PASSWORD,
                initial_folder="meziere",
            ) as mailbox:
                logging.info("Connected to %s", IMAP_HOST)

                # Process emails that arrived while the program was offline.
                process_unread_messages(mailbox)

                while True:
                    # Wait for a mailbox change. The timeout periodically
                    # refreshes the IDLE connection.
                    responses = mailbox.idle.wait(timeout=180)

                    if responses:
                        process_unread_messages(mailbox)

        except KeyboardInterrupt:
            logging.info("Listener stopped")
            return

        except Exception:
            logging.exception("Mailbox connection failed; reconnecting")
            time.sleep(10)


if __name__ == "__main__":
    logging.basicConfig(
        level=logging.INFO,
        format="%(asctime)s %(levelname)s %(message)s",
    )

    listen_for_email()