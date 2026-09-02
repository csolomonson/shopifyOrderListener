import asyncio
import unittest
from unittest.mock import patch

from web.app import lifespan


class LifespanTests(unittest.IsolatedAsyncioTestCase):
    async def test_startup_sync_does_not_block_application_readiness(self):
        worker_started = asyncio.Event()

        async def blocked_worker(**_kwargs):
            worker_started.set()
            await asyncio.Event().wait()

        with (
            patch("web.app.boolean_setting", return_value=True),
            patch("web.app._shopify_worker", side_effect=blocked_worker),
        ):
            async with lifespan(None):
                await asyncio.wait_for(worker_started.wait(), timeout=1)


if __name__ == "__main__":
    unittest.main()
